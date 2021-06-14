-- Escort Stealth Dragon, Hayashi Kaze

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnGC, false, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnGC() and obj.CanDiscard(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.Discard(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.PerfectGuard()
	end
	return 0
end