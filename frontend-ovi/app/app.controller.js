app.controller('searchController', ['$scope', 'manipulateData', 'getRequestTitle', ($scope, manipulateData, getRequestTitle) => {
    $scope.fetchData = (title) => {
        if (title) {
            $scope.details = !$scope.details;
            getRequestTitle.get(title).then(
                (response) => {
                    $scope.datas = manipulateData.getMovieInfo(JSON.stringify(response.data));
                    $scope.propertiesNumber = Object.keys($scope.datas).length;
                },
                (error) => {
                    $scope.datas = error.statusText;
                }
            );
        }
    }

    $scope.clear = () => {
        $scope.datas = [];
        $scope.title = '';
        $scope.propertiesNumber = 0;
    }
}]);
