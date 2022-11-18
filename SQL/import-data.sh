for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P dhd743hf_g83hdf_gsd89g -d master -i create-corgishop.sql
    if [ $? -eq 0 ]
    then
        echo "create-corgishop.sql completed"
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done