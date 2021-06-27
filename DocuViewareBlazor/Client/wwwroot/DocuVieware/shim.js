window.docuViewareShim = {
    loadControl: (target, html) => {
        $(target).html(html);
    }
};

window.docuViewareInterop = {
    GetPageCount: (docuViewareID) => {
        return DocuViewareAPI.GetPageCount(docuViewareID);
    },

    GetCurrentPage: (docuViewareID) => {
        return DocuViewareAPI.GetCurrentPage(docuViewareID);
    },

    SelectPage: (docuViewareID, pageNo) => {
        DocuViewareAPI.SelectPage(docuViewareID, pageNo);
    },

    RegisterCustomEvent: (docuViewareID, dotNetHelper) => {
        if (typeof DocuViewareAPI !== "undefined" && DocuViewareAPI.IsInitialized(docuViewareID)) {
            DocuViewareAPI.RegisterOnNewDocumentLoaded(docuViewareID, function () {
                var pageCount = DocuViewareAPI.GetPageCount(docuViewareID);
                var currentPage = DocuViewareAPI.GetCurrentPage(docuViewareID);
                dotNetHelper.invokeMethod('DocuViewareDocumentOnLoaded', pageCount, currentPage);
            });

            DocuViewareAPI.RegisterOnPageChanged(docuViewareID, function () {
                var currentPage = DocuViewareAPI.GetCurrentPage(docuViewareID);
                dotNetHelper.invokeMethod('RegisterOnPageChanged', currentPage);
            });

            DocuViewareAPI.RegisterOnAreaSelected(docuViewareID, function () {
                var coordinates = DocuViewareAPI.GetSelectionAreaCoordinates(docuViewareID);
                dotNetHelper.invokeMethod('RegisterOnAreaSelected', coordinates.left, coordinates.top, coordinates.width, coordinates.height);
            });
        }
        else {
            setTimeout(function () { RegisterOnNewDocumentLoadedOnDocuViewareAPIReady() }, 10);
        }
    }
};

