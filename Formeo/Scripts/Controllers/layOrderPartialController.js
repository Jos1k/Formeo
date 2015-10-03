var layOrderPartialController = function ($scope, $modalInstance, $window) {

    $scope.asd = "Hi";

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}