var formeoAngularMVCApp = angular.module('formeoAngularMVCApp', ['ngFileUpload', 'ui.bootstrap']);

//formeoAngularMVCApp.controller('commonController', commonController);
formeoAngularMVCApp.controller('adminPageController', adminPageController);
formeoAngularMVCApp.controller('customerPageController', customerPageController);
formeoAngularMVCApp.controller('producerPageController', producerPageController);

formeoAngularMVCApp.controller('layOrderPartialController', layOrderPartialController);
formeoAngularMVCApp.controller('layOrderConfirmPartialController', layOrderConfirmPartialController);
formeoAngularMVCApp.controller('addProductsPartialController', addProductsPartialController);
formeoAngularMVCApp.controller('bidsForPrintObjectController', bidsForPrintObjectController);
formeoAngularMVCApp.controller('uploadProductController', uploadProductController);
formeoAngularMVCApp.controller('bidProductPartialController', bidProductPartialController);


formeoAngularMVCApp.factory('UploadPrinObject', function ($window, $http, $modal) {
    var root = {};
    root.showUpload = function (parentPrintObjectList) {
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
                parentPrintObjectList.push(response);
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