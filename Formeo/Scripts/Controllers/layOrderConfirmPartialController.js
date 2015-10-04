var layOrderConfirmPartialController = function ($scope, $modalInstance, $window, $modal, $http, printObjectsInfoModal, deliveryInfo) {

    $scope.asd = "Hi";
    $scope.deliveryInfo = deliveryInfo;
    $scope.printObjectsInfoModal = printObjectsInfoModal;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.CreateOrder = function () {

        var infosParam = [];
        $scope.printObjectsInfoModal.forEach(function (item, i, arr) {
            infosParam.push
                ({
                    PrintObjectId: item.PrintObjectId,
                    //ArtNo: item.ArtNo,
                    //Name: item.Name,
                    Quantity: item.Quantity
                });
        });
        $http({
            method: 'POST',
            url: '/Project/CreateOrder',
            params: { 'printObjectInfo': JSON.stringify(infosParam), 'deliveryInfo': $scope.deliveryInfo },
            headers: { 'Content-Type': 'application/json;' },
            data: ''
        }).
            then(function (response) {
                //success

               // $window.alert('success');


            }, function (response) {
                //error
                $window.alert('error');

            });

        $modalInstance.dismiss('cancel');
    };

}