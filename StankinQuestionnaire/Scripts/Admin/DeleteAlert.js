function DeleteAlert() {
    var attr = {
        deleteUrl: "data-delete-url",
        id: "data-id"
    }
    var currentID = -1;

    var selectors = {
        alert: "#delete-form",
        btnConfirm: "#btnConfirmDelete",
        btnDelete: ".btn-delete"
    }

    var settings = {
        deleteUrl: $(selectors.alert).attr(attr.deleteUrl)
    }

    $(selectors.btnDelete).click(function () {
        currentID = $(this)
              .parents("tr:first")
              .attr(attr.id);
        $(selectors.alert).modal('show');
    });

    $(selectors.btnConfirm).click(function () {
        $.ajax({
            url: settings.deleteUrl,
            data: { id: currentID },
            method: "POST"
        })
       .always(function () {
       })
       .done(function (data) {
           if (data.Status === 1) {
               var currentTr = $('tr[' + attr.id + '="' + data.ID + '"]');
               if (currentTr.length > 0) {
                   $(currentTr).remove();
                   $(selectors.alert).modal('hide');
               }
           }
       });

    });
}
//$(function () {
(function () {
    var delAlert = new DeleteAlert();
})()
//})
