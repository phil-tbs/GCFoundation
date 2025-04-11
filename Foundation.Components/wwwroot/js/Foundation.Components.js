document.querySelectorAll('.tabulator-table').forEach(el => {
    console.log(el);
    const config = {
        layout: el.dataset.layout || "fitColumns",
        pagination: true,
        paginationSize: parseInt(el.dataset.paginationSize || "10"),
        columns: []
    };

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
        //config.progressiveLoad = "load";
        config.ajaxConfig = "POST";
        config.ajaxContentType = "json";
        config.paginationMode = "remote";
    }

    console.log(config);

    new Tabulator(el, config);
});