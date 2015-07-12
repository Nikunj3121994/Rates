$(function () {
    var selectors = {
        btnAdd: "#btn-add-rank",
        line: ".rank-line",
        searchContainer: '.control-group',
        tempContainer: '#template-container',
        btnDelete: '.btn-delete'
    }

    var attrs = {
        index: 'data-index'
    }

    $(selectors.btnAdd).on('click', function () {
        var newLine = getNewLine();
        $(selectors.searchContainer).append(newLine);
    })

    function getLastIndex() {
        var lastLine = $(selectors.searchContainer).find(selectors.line + ':last');
        var currentLastIndex = parseInt(lastLine.attr(attrs.index));
        return currentLastIndex ? currentLastIndex : 0;
    }

    function getNewLine() {
        var lastIndex = getLastIndex();
        var newLine = $(selectors.tempContainer).html().replace(/%0/g, lastIndex + 1);
        return newLine;
    }

    $(selectors.searchContainer).on('click', selectors.btnDelete, function () {
        var lineForRemove = $(this).parents(selectors.line + ":first");
        lineForRemove.remove();
    });
})