var adminPageController = function ($scope) {
    $scope.helloVariable = 'I work!';
    $scope.mainMenu = []
    $scope.selectMainMenu = function (item) {
        $scope.selected = item;
    };

    $scope.isActive = function (item) {
        return $scope.selected === item;
    };



}