UPDATE MAIN_DATA_VERIFIED
SET item = CASE
               WHEN item IS NULL OR item = '' THEN 'LADIES RING'
               ELSE item
           END;