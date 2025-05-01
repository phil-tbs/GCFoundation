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
            console.log(table.getColumns());
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
