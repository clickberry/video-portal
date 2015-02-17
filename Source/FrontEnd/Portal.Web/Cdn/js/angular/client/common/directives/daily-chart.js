// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

angular.module('directives.clients.chart.daily', []).directive('cbDailyChart', [
    'highcharts', function(highcharts) {

        return {
            restrict: 'E',
            replace: true,
            scope: {
                items: '=',
                itemsName: '@',
                title: '@'
            },
            template: '<div id="container" style="margin: 0 auto; height: 100%">Chart not available.</div>',
            link: function(scope /*, element, attrs*/) {

                var chart = new highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        backgroundColor: null
                    },
                    credits: false, // remove brand link
                    title: {
                        text: scope.title
                    },
                    xAxis: {
                        type: 'datetime',
                        labels: {
                            formatter: function () {

                                var date = new Date(this.value);

                                // skipping intermediate values
                                if (date.getUTCHours() > 0) {
                                    return '';
                                }

                                return Highcharts.dateFormat('%b %e', date);
                            }
                        }
                    },
                    yAxis: {
                        title:
                        {
                            text: null
                        },
                        floor: 0
                    },
                    tooltip: {
                        formatter: function () {
                            var name = scope.itemsName ? scope.itemsName.toLocaleLowerCase() + ' ' : '';
                            return this.y + ' ' + name + 'at ' + Highcharts.dateFormat('%b %e, %Y', new Date(this.x));
                        }
                    },
                    series: [
                        {
                            type: 'line',
                            color: '#45a64f',
                            marker: {
                                radius: 10
                            },
                            lineWidth: 5,
                            showInLegend: false,
                            name: scope.itemsName,
                            data: scope.items
                        }
                    ]
                });


                // watching for updates
                scope.$watch("items", function(newValue) {
                    chart.series[0].setData(newValue, true);
                }, true);

                // destructor
                scope.$on('$destroy', function() {
                    chart.destroy();
                });
            }
        };
    }
])