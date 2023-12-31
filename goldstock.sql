SELECT SUM(GW) AS TotalGW, SUM(NW) AS TotalNW
FROM MAIN_DATA
WHERE TAG_NO NOT LIKE '%SYA%'
AND (
    TAG_NO NOT LIKE 'SKS%'
    AND TAG_NO NOT LIKE 'BKS%'
    AND TAG_NO NOT LIKE 'KDS%'
    AND TAG_NO NOT LIKE 'BCS%'
    AND TAG_NO NOT LIKE 'BPS%'
    AND TAG_NO NOT LIKE 'CHS%'
    AND TAG_NO NOT LIKE 'FNS%'
    AND TAG_NO NOT LIKE 'JKS%'
    AND TAG_NO NOT LIKE 'JRS%'
    AND TAG_NO NOT LIKE 'LJS%'
    AND TAG_NO NOT LIKE 'LRS%'
    AND TAG_NO NOT LIKE 'MSS%'
    AND TAG_NO NOT LIKE 'MPS%'
    AND TAG_NO NOT LIKE 'OSS%'
    AND TAG_NO NOT LIKE 'ORS%'
    AND TAG_NO NOT LIKE 'PYS%'
    AND TAG_NO NOT LIKE 'PJS%'
    AND TAG_NO NOT LIKE 'SDS%'
    AND TAG_NO NOT LIKE 'SGS%'
    AND TAG_NO NOT LIKE 'SMS%'
    AND TAG_NO NOT LIKE 'SVS%'
    AND TAG_NO NOT LIKE 'SRS%'
    AND TAG_NO NOT LIKE 'ZDS%'
);
