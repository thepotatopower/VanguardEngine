-- Violate Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 2
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnGC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnGC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		if obj.Exists(2) then
			obj.Discard(1)
		end
	end
end

function Activate(n)
	if n == 1 then
		obj.PerfectGuard()
	end
	return 0
end