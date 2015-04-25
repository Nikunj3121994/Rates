angular.module('arrayLengthFilter', []).filter('viewLength', function () {
    return function (arrayLength) {
        return arrayLength > 0 ? "(" + arrayLength + ")" : '';
    };
});

angular.module('maxPointFilter', []).filter('maxPoint', function () {
    return function (maxPoint) {
        return maxPoint ? (maxPoint > 0 ? "(Максимальное количество " + maxPoint+")" : '') : "";
    };
});