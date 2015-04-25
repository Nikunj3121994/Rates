var documentServices = angular.module('documentServices', ['ngResource']);

documentServices.factory('AddCalculation', ['$resource',
    function ($resource) {
        return $resource('AddCalculation', {}, {
            query: { method: 'POST', params: { calculation: null } }
        });
    }]);

documentServices.factory('UpdateCalculation', ['$resource',
    function ($resource) {
        return $resource('UpdateCalculation', {}, {
            query: { method: 'POST', params: { calculation: null } }
        });
    }]);

documentServices.factory('DeleteCalculation', ['$resource',
    function ($resource) {
        return $resource('DeleteCalculation', {}, {
            query: { method: 'POST', params: { calculationID: null, documentID: null } }
        });
    }]);