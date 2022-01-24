app.controller('searchController', ['$scope', 'getRequestTitle', 'getRequestMovieInfo', 'manipulateData', ($scope, getRequestTitle, getRequestMovieInfo, manipulateData) => {
    $scope.fetchData = (title, filter) => {
        $scope.details = !$scope.details;
        if (filter === "title") {
            getRequestTitle.get(title).then(
                (response) => {
                    $scope.datas = response.data.results.bindings;
                    $scope.propertiesNumber = Object.keys($scope.datas).length;
                },
                (error) => {
                    $scope.datas = error.statusText;
                });
        } else {
            getRequestMovieInfo.get(title).then(
                (response) => {
                    $scope.datas = manipulateData.getMovieInfo(JSON.stringify(response.data));
                    $scope.propertiesNumber = Object.keys($scope.datas).length;
                },
                (error) => {
                    $scope.datas = error.statusText;
                });
        }
    }

    $scope.clear = () => {
        $scope.datas = [];
        $scope.title = '';
    }
}]);
