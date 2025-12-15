        var fileQuery = from file in fileList
                        let fileLen = new FileInfo(file).Length
                        where fileLen > 0
                        select fileLen;