-- Sunrise Egg

function ConditionType()
	return e.OnRide, 0
end

function CheckConditionOnRide()
	if obj.isTopSoul() and obj.Turn() > 1 then
		return true
	else
		return false
	end
end

function OnRideActivate(n)
	obj.Draw(1)
end