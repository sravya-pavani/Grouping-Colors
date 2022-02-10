# Grouping-Colors

## Overview

* This is a sample application based on ASP.NET MVC that retrieves colors 
from a sample Web API - https://reqres.in/api/example
* The API contains different pages of colors.
* The entire data is merged and grouped into three lists based on the following rules:
  * Group One - if the first part of pantone_value field is divisible by 3
  * Group Two - if the first part of pantone_value is divisible by 2 and doesn't exists in group one
  * Group Three - the colors which doesn't exists either in group one or group two
* The data is populated on a web page with all the groups.
