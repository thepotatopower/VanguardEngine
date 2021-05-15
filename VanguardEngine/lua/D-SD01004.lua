-- Sunrise Egg

function NumberOfEffects()
	return 1
end

function NumberOfParams(n)
	if n == 1 then
		return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, l.TopSoul, false, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsTopSoul() and obj.Turn() > 1 then
			return true
		end
	end
	return false
end

function Activate(n, i)
	if n == 1 then
		obj.Draw(1)
	end
	return 0
end