var documentApp = angular.module('documents', ['arrayLengthFilter', 'documentServices', 'maxPointFilter']);

//function IndicatorGroup(indicatorGroupID, name, indicators) {
//    this.IndicatorGroupID = indicatorGroupID;
//    this.Name = name;
//    this.Indicators = indicators;
//}

//function Indicator(indicatorID, name, maxPoint, calculationTypes) {
//    this.IndicatorID = IndicatorID;
//    this.Name = name;
//    this.MaxPoint = maxPoint;
//    this.CalculationTypes = calculationTypes;
//}

//function CalculationType(calculationTypeID, unitName, point, calculations) {
//    this.CalculationTypeID = CalculationTypeID;
//    this.UnitName = UnitName;
//    this.Point = Point;
//    this.Calculations = Calculations;
//}

//function Сalculation(calculationID, calculationTypeID) {
//    this.CalculationID = calculationID;
//    this.CalculationTypeID = calculationTypeID;
//}
documentApp.controller('DocumentType', ['$scope', '$http', 'AddCalculation', 'DeleteCalculation', 'UpdateCalculation', function ($scope, $http, AddCalculation, DeleteCalculation, UpdateCalculation) {
    selectors = {
        addForm: "#add-calculation",
        viewCalculation: "#view-calculation"
    }
    $scope.addForm = {
        title: "",
        calculationTypeID: "",
        description: ""
    }
    $scope.addError = true;
    $scope.updateError = true;

    $http.get('DocumentJSON?documentID=' + documentData.documentID).success(function (data) {
        $scope.document = data;
        $scope.document.Point = getCurrentPoint();
    });

    $scope.btnAdd = function (unitName, calculationTypeID) {
        $scope.addForm.title = unitName;
        $scope.addForm.calculationTypeID = calculationTypeID;
        $scope.addError = true;
        $(selectors.addForm).modal('show');
    }

    $scope.saveAdd = function () {
        AddCalculation.save({
            calculation: {
                description: $scope.addForm.description,
                calculationTypeID: $scope.addForm.calculationTypeID,
                documentID: $scope.document.DocumentID
            },
        },
        function (calculation) {
            var calculationType = findCalcaultionType(calculation.CalculationTypeID);
            if (calculationType != null) {
                calculationType.Calculations.push({
                    CalculationID: calculation.CalculationID,
                    CalculationTypeID: calculation.CalculationTypeID,
                    Description: calculation.Description
                });
                $scope.document.Point = getCurrentPoint();
                $(selectors.addForm).modal('hide');
                $scope.addForm.description = "";
            }
        },
        function (error) {
            $scope.addError = false;
        });
    }

    $scope.viewCalculations = function (calcType) {
        $scope.currentCalculations = calcType.Calculations;
        $scope.updateError = true;
        $scope.updateSuccess = true;
        $scope.deleteSuccess = true;
        $(selectors.viewCalculation).modal('show');
    }

    $scope.saveCalculation = function (calculation) {
        console.log(calculation);
        UpdateCalculation.save({
            calculation: calculation
        },
        function (saveCalculation) {
            $scope.updateSuccess = false;
            var calculationType = findCalcaultionType(saveCalculation.CalculationTypeID);
            if (calculationType != null) {
                var fndCalculation = findCalculation(saveCalculation.CalculationID, calculationType);
                if (fndCalculation != null) {
                    fndCalculation.Description = saveCalculation.Description;
                }
            }
        }
        , function (error) {
            $scope.updateError = false;
        });
    }

    $scope.deleteCalculation = function (calculation) {
        DeleteCalculation.save({
            calculationID: calculation.CalculationID,
            documentID: documentData.documentID
        },
        function (success) {
            $scope.deleteSuccess = false;
            var fndCalculationType = findCalculationTypeByCalculation(success.calculationID);
            var index = findCalculationIndex(success.calculationID, fndCalculationType);
            if (index !== null) {
                fndCalculationType.Calculations.splice(index, 1);
                $scope.document.Point = getCurrentPoint();
                if (fndCalculationType.Calculations.length < 1) {
                    $(selectors.viewCalculation).modal('hide');
                }
            }
        })
    }

    function findCalcaultionType(calculationTypeID) {
        var currentDocument = $scope.document;
        for (var i = 0; i < currentDocument.IndicatorGroups.length; i++) {
            for (var j = 0; j < currentDocument.IndicatorGroups[i].Indicators.length; j++) {
                for (var g = 0; g < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes.length; g++) {
                    if (currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].CalculationTypeID == calculationTypeID) {
                        return currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g];
                    }
                }
            }
        }
        return null;
    }
    function findCalculationTypeByCalculation(calculationID) {
        var currentDocument = $scope.document;
        for (var i = 0; i < currentDocument.IndicatorGroups.length; i++) {
            for (var j = 0; j < currentDocument.IndicatorGroups[i].Indicators.length; j++) {
                for (var g = 0; g < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes.length; g++) {
                    for (var h = 0; h < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations.length; h++) {
                        if (currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations[h]
                            && currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations[h].CalculationID == calculationID) {
                            return currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g];
                        }
                    }
                }
            }
        }
    }
    function findCalculationIndex(calculationID, calculationType) {
        var currentDocument = $scope.document;
        console.log(currentDocument);
        for (var i = 0; i < calculationType.Calculations.length; i++) {
            if (calculationType.Calculations[i].CalculationID == calculationID) {
                return i;
            }
        }

    }
    function findCalculation(calculationID, calculationType) {
        var currentDocument = $scope.document;
        if (calculationType) {
            for (var i = 0; i < calculationType.Calculations.length; i++) {
                if (calculationType.Calculations[i].CalculationID == calculationID) {
                    return calculationType.Calculations[i];
                }
            }
        }
        else {
            for (var i = 0; i < currentDocument.IndicatorGroups.length; i++) {
                for (var j = 0; j < currentDocument.IndicatorGroups[i].Indicators.length; j++) {
                    for (var g = 0; g < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes.length; g++) {
                        for (var h = 0; h < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations.length; h++) {
                            if (currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations[h]
                                && currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations[h].CalculationID == calculationID) {
                                return currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations[h];
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    function getCurrentPoint() {
        var allPoint = 0;
        var currentDocument = $scope.document;
        currentDocument.IndicatorGroups.forEach(function (indicatorGroup, index) {
            var indicatorGroupPoint = 0;
            indicatorGroup.Indicators.forEach(function (indicator, index) {
                var indicatorPoint = 0;
                indicator.CalculationTypes.forEach(function (calculationType, index) {
                    var pointForOne = calculationType.Point;
                    var calculationTypePoint = pointForOne * calculationType.Calculations.length;
                    if (calculationType.MaxPoint) {
                        if (calculationTypePoint > calculationType.MaxPoint) {
                            calculationTypePoint = calculationType.MaxPoint;
                        }
                    }
                    indicatorPoint += calculationTypePoint;
                })
                indicatorGroupPoint += indicatorPoint;
            })
            if (indicatorGroupPoint > indicatorGroup.MaxPoint) {
                indicatorGroupPoint = indicatorGroup.MaxPoint;
            }
            allPoint += indicatorGroupPoint;
        })
        if (allPoint > documentData.maxPoint) {
            allPoint = documentData.maxPoint;
        }
        return allPoint;

        //for (var i = 0; i < currentDocument.IndicatorGroups.length; i++) {
        //    for (var j = 0; j < currentDocument.IndicatorGroups[i].Indicators.length; j++) {
        //        for (var g = 0; g < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes.length; g++) {
        //            point+=
        //            for (var h = 0; h < currentDocument.IndicatorGroups[i].Indicators[j].CalculationTypes[g].Calculations.length; h++) {

        //            }
        //        }
        //    }
        //}
    }
}]);