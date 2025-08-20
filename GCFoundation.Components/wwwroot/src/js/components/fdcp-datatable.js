document.querySelectorAll('.tabulator-table').forEach(el => {
    const config = {
        layout: el.dataset.layout || "fitColumns",
        pagination: true,
        paginationSize: parseInt(el.dataset.paginationSize || "10"),
        columns: [],
        height: '100%'
    };

    // Parse columns
    if (el.dataset.columns) {
        try {
            config.columns = JSON.parse(el.dataset.columns);
        } catch (e) {
            console.error("Failed to parse columns:", e);
        }
    }

    if (el.dataset.set) {
        config.data = JSON.parse(el.dataset.set);
    } else if (el.dataset.ajaxurl) {
        config.ajaxURL = el.dataset.ajaxurl;
        config.filterMode = "remote";
        config.sortMode = "remote";
        config.ajaxConfig = "POST";
        config.ajaxContentType = "json";
        config.paginationMode = "remote";
        config.filterMode = "remote";

        // Add anti-forgery token to AJAX requests if available
        if (el.dataset.antiforgeryToken) {
            // Parse the anti-forgery token HTML to extract the token value
            const parser = new DOMParser();
            const tokenDoc = parser.parseFromString(el.dataset.antiforgeryToken, 'text/html');
            const tokenInput = tokenDoc.querySelector('input[name="__RequestVerificationToken"]');
            
            if (tokenInput) {
                const tokenValue = tokenInput.getAttribute('value');
                config.ajaxRequestFunc = function(url, config, params) {
                    return fetch(url, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': tokenValue
                        },
                        body: JSON.stringify(params)
                    }).then(response => response.json());
                };
            }
        }
    }


    new Tabulator(el, config);
});

document.querySelectorAll('.tabulator-search-input').forEach(el => {
    el.addEventListener("input", debounce(function (e) {
        let tabulatorId = e.target.dataset.tabulatorId;
        let table = Tabulator.findTable("#" + tabulatorId)[0];
        const value = e.target.value.trim();

        if (!table) return;

        if (value === "") {
            table.clearFilter();
        } else {
            const fields = table.getColumns()
                .filter(col => {
                    const def = col.getDefinition();
                    return def.filter; // Only fields with headerFilter enabled
                })
                .map(col => col.getField());


            table.setFilter(
                fields.map(field => ({ field: field, type: "like", value: value }))
            );
        }
    }, 200)); // 200ms debounce
});

function debounce(func, wait) {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), wait);
    };
}
