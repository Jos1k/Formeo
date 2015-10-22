var formeoAngularMVCApp = angular.module('formeoAngularMVCApp', ['ngFileUpload', 'ui.bootstrap', 'ui.select', 'ngSanitize']);

formeoAngularMVCApp.controller('adminPageController', adminPageController);
formeoAngularMVCApp.controller('customerPageController', customerPageController);
formeoAngularMVCApp.controller('producerPageController', producerPageController);

formeoAngularMVCApp.controller('layOrderPartialController', layOrderPartialController);
formeoAngularMVCApp.controller('layOrderConfirmPartialController', layOrderConfirmPartialController);
formeoAngularMVCApp.controller('addProductsPartialController', addProductsPartialController);
formeoAngularMVCApp.controller('bidsForPrintObjectController', bidsForPrintObjectController);
formeoAngularMVCApp.controller('uploadProductController', uploadProductController);
formeoAngularMVCApp.controller('bidProductPartialController', bidProductPartialController);
formeoAngularMVCApp.controller('companyEditController', companyEditController);
formeoAngularMVCApp.controller('orderInfoPartialController', orderInfoPartialController);
formeoAngularMVCApp.controller('printObjectDetailsPartialController', printObjectDetailsPartialController);


formeoAngularMVCApp.directive('uiSelectField', function () {
    return {
        restrict: 'A',
        require: ['uiSelect', 'ngModel'],
        link: function (scope, element, attrs, ctrls) {
            var $select, ngModel;
            $select = ctrls[0];
            ngModel = ctrls[1];
            return ngModel.$parsers.push(function (value) {
                if (value[attrs.uiSelectField]) return value[attrs.uiSelectField];
                else return value;
            });
        }
    };
});

formeoAngularMVCApp.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            var keys = Object.keys(props);

            items.forEach(function (item) {
                var itemMatches = false;

                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
});

formeoAngularMVCApp.factory('UploadPrinObject', function ($window, $http, $modal, $timeout) {
    var root = {};
    var scope = {};

    root.storeScope = function (scopeInstance, propertyName) {
        scope.instance = scopeInstance;
        scope.printObjectsProperty = propertyName;
    };

    root.download = function (printObjectId) {
        return $http({
            url: '/PrintObjects/Download',
            method: 'POST',
            data: { printObjectId: printObjectId },
            responseType: 'blob'
        })
            .then(function (response) {
                try {
                    $timeout(function () {
                        saveAs(response.data, "printobjectProduct.pdf");
                    });
                } catch (ex) {
                    console.log(ex);
                }
            }, function (response) {
                // error
                //return $q.reject(response.data);
            });
    }

    root.showUpload = function () {
        $http({
            method: 'GET',
            url: '/PrintObjects/UploadProductShowModal',
            //params: { 'selectedPrintObjectIds': $scope.selectedPrintObjectIds },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
        then(function (response) {
            //success
            var modalInstance = $modal.open({
                template: (response.data),
                controller: 'uploadProductController',
                backdrop: 'static',
                windowClass: 'app-modal-window'
            });


            modalInstance.result.then(function (response) {
                //modal success
                var products = JSON.parse(response);
                if (products && products.length != 0) {
                    for (var i = 0; i < products.length; i++) {
                        scope.instance[scope.printObjectsProperty].push(products[i]);
                        scope.instance['selectedMainMenu'] = '/Storage';
                    }
                }
            }, function (response) {
                //modal error
                // $window.alert('error uploading files');

            });
        }, function (response) {
            //error or dismiss
            // $window.alert('error opening modal window');
        });

        //everything is bad
        return null;
    }
    return root;
});

formeoAngularMVCApp.config(['$httpProvider', function ($httpProvider) {
    //initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
}]);