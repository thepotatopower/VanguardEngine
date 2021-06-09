-- Sunrise Egg

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, l.Soul, false, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.Turn() > 1 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Draw(1)
		obj.OnRideAbilityResolved()
	end
	return 0
end