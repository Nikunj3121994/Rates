angular.module('arrayLengthFilter', []).filter('viewLength', function () {
    return function (arrayLength) {
        return arrayLength > 0 ? "("+arrayLength+")" : '';
    };
});