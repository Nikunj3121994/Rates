$(function () {
    var selectors = {
        btnDelete: "#btn-review-delete",
        reviewLine: ".review-line",
        btnAdd: "#btn-add",
        docTypeSelect: ".document-type-select",
        template: "#template",
        lines: "#review-lines",
        docTypeTemplate: "#doc-type-select"
    }

    var attrs = {
        documentID: "data-document-typeid"
    }
    var templaties = {
        line: '<div class="form-inline review-line">\
            <div class="form-group">\
                <label>Документ</label>\
                <select class="form-control document-type-select">\
                %1\
                </select>\
            </div>'+
            '<div class="form-group">\
                <label>Группа показателей</label>\
                <select class="form-control" name="SelectedIndicatorGroupsID">\
                    %2\
                </select>\
            </div>\
            <div class="form-group">\
                <button type="button" class="btn btn-danger" id="btn-review-delete">\
                    <span class="glyphicon glyphicon-trash"></span>\
                </button>\
            </div>\
            </div>'
    }

    $(selectors.lines).on("click", selectors.btnDelete, function () {
        $(this).parents(selectors.reviewLine).remove();
    });
    $(selectors.btnAdd).on('click', function () {
        var docTypeOptions = $(selectors.docTypeTemplate).html();
        var selectDocType = $(selectors.docTypeTemplate).find("option:first");
        var indicatorGroupOptions = $(selectors.template).find("select[" + attrs.documentID + "='" + selectDocType.prop("value") + "']").html();
        var line = templaties.line
            .replace(/%1/, docTypeOptions)
            .replace(/%2/, indicatorGroupOptions);
        $(selectors.lines).append(line);
    })

    $(selectors.lines).on('change', selectors.docTypeSelect, function () {
        var docTypeID = parseInt($(this).find("option:selected").prop("value"));
        var newOptions = $(selectors.template).find("select[" + attrs.documentID + "='" + docTypeID + "']").html();
        $(this).parents(selectors.reviewLine)
            .find("select[name='SelectedIndicatorGroupsID']").html(newOptions);
    })
})