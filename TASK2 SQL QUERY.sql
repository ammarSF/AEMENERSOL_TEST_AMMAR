WITH latest_updated AS (
	SELECT p.uniqueName as PlatformName, p.Id, p.id as PlatformId, w.UniqueName, w.Latitude, w.Longitude, w.CreatedAt, w.UpdatedAt, ROW_NUMBER() OVER (PARTITION BY p.uniqueName ORDER BY w.updatedAt DESC) AS rn
	FROM PlatformTables AS p
	JOIN WellTables AS w ON w.platformId = p.id
)
SELECT * FROM latest_updated WHERE rn = 1;