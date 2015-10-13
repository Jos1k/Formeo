var bidsForPrintObjectController = function ($scope, $http, $window, $modalInstance, printObjectId) {

    $scope.printObjectId = printObjectId;
    $scope.selectedProducerCompanyId = null;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.HasSelectedProducer = function () {
        return $scope.selectedProducerCompanyId != null;
    }

    $scope.selectSingleItem = function (position, entities) {
        angular.forEach(entities, function (bid, index) {
            if (position != index) {
                bid.IsSelected = false;
            }
            else {
                $scope.selectedProducerCompanyId = bid.ProducerCompanyId;
            }
        });
    };

    $scope.SaveSelectProducerCompany = function () {

        if ($scope.selectedProducerCompanyId == null) {
            $window.alert('Error: Producer not selected');
            return;
        }

        $http({
            method: 'POST',
            url: '/PrintObjects/AssingProducerToPrintObject',
            params: { 'printObjectId': $scope.printObjectId, 'producerCompanyId': $scope.selectedProducerCompanyId },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        }).
                then(function (response) {
                    //success
                    //no data here from responce
                    $modalInstance.close($scope.selectedProducerCompanyId);
                }, function (response) {
                    //http error
                    $modalInstance.dismiss("close");
                    $window.alert('error');
                });
    }

    $scope.removeItemById = function (elements, id) {
        angular.forEach(elements, function (element, index) {
            if (element.Id == id) {
                elements.splice(index, 1);
            }
        });
    }
}