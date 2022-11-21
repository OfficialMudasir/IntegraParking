// Kendo UI theme for data visualization widgets
// Use as theme: 'newTheme' in configuration options (or change the name)
kendo.dataviz.ui.registerTheme('newTheme', {
    "chart": {
        "title": {
            "color": "#8a8a8a"
        },
        "legend": {
            "labels": {
                "color": "#a8a8a8"
            }
        },
        "chartArea": {
            "background": ""
        },
        "seriesDefaults": {
            "labels": {
                "color": "#8a8a8a"
            }
        },
        "axisDefaults": {
            "line": {
                "color": "rgba(0, 60, 130, 0.66)"
            },
            "labels": {
                "color": "rgba(0, 60, 130, 0.66)"
            },
            "minorGridLines": {
                "color": "rgba(0, 60, 130, 0.15)"
            },
            "majorGridLines": {
                "color": "rgba(0, 60, 130, 0.33)"
            },
            "title": {
                "color": "#8a8a8a"
            }
        },
        "seriesColors": [
            "#00448f",
            "#01cbbf",
            "#339933",
            "#f2b661",
            "#e67d4a",
            "#da3b36"
        ]
    },
    "gauge": {
        "pointer": {
            "color": "rgba(0, 60, 130, 0.8)"
        },
        "scale": {
            "rangePlaceholderColor": "rgba(0, 60, 130, 0.2)",
            "labels": {
                "color": "rgba(0, 60, 130, 0.8)"
            },
            "minorTicks": {
                "color": "rgba(0, 60, 130, 0.33)"
            },
            "majorTicks": {
                "color": "rgba(0, 60, 130, 0.66)"
            },
            "line": {
                "color": "rgba(0, 60, 130, 0.8)"
            }
        }
    }
});