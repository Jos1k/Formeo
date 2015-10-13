var formeoAngularMVCApp = angular.module('formeoAngularMVCApp', ['ngFileUpload', 'ui.bootstrap']);

formeoAngularMVCApp.controller('commonController', commonController);
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
                return response;
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
