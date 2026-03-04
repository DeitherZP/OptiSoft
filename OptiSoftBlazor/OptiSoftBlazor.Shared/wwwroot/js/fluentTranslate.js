window.translateFluentLabels = function () {

    /* ===============================
       1. PAGINADOR
       =============================== */
    function applyPaginatorTranslations() {
        document.querySelectorAll(".paginator nav .pagination-text").forEach(el => {

            if (el.dataset.translated === "true") return;

            // Recorremos SOLO nodos de texto
            el.childNodes.forEach(node => {
                if (node.nodeType === Node.TEXT_NODE) {
                    node.textContent = node.textContent
                        .replace("Page ", "Página ")
                        .replace(" of ", " de ");
                }
            });

            el.dataset.translated = "true";
        });
    }

    function waitForPaginator(attempt = 0) {
        const exists = document.querySelector(".paginator nav .pagination-text");

        if (exists) {
            applyPaginatorTranslations();

            const observer = new MutationObserver(() => {
                applyPaginatorTranslations();
            });

            observer.observe(exists, {
                childList: true,
                subtree: true
            });
        }
        else if (attempt < 20) {
            setTimeout(() => waitForPaginator(attempt + 1), 200);
        }
    }

    /* ===============================
       2. DATA GRID – SIN REGISTROS
       =============================== */
    function applyEmptyGridTranslation() {
        document
            .querySelectorAll(
                ".data-grid-table table tbody tr.empty-content-row td.empty-content-cell"
            )
            .forEach(el => {

                if (el.dataset.translated === "true") return;

                if (el.textContent.trim() === "No data to show!") {
                    el.textContent = "No hay datos para mostrar";
                }

                el.dataset.translated = "true";
            });
    }

    /* ===============================
       3. OBSERVADOR GENERAL
       =============================== */
    function observeChanges() {

        applyPaginatorTranslations();
        applyEmptyGridTranslation();

        const observer = new MutationObserver(() => {
            applyPaginatorTranslations();
            applyEmptyGridTranslation();
        });

        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
    }

    // Inicialización
    waitForPaginator();
    observeChanges();
};