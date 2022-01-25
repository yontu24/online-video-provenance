app.component('searchMovieForm', {
    transclude: true,
    controller: function(manipulateData, getRequestTitle) {
        var self = this;

        self.fetchData = (title) => {
            if (title) {
                getRequestTitle.get(title).then(
                    (response) => {
                        self.datas = manipulateData.getMovieInfo(JSON.stringify(response.data));
                        self.propertiesNumber = Object.keys(self.datas).length;
                    },
                    (error) => {
                        self.datas = error.statusText;
                    }
                );

                self.datas = '';
                self.propertiesNumber = 0;
                self.title = '';
            }
        };
    },
    controllerAs: 'searchmovie',
    templateUrl: 'http://localhost:8080/ovi/app/searchMovie/searchMovie.html'
});
