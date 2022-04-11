-- Lovingly Watching Over, Otirie

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Name, "Truehearted Ruby", q.Other, o.Player, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Name, "Prismajica, Wilista", q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrderPlayed, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.Exists(1) and obj.Exists(2) then
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

function Activate(n)
	if n == 1 then
		obj.AddTempPower(3, 10000)
		obj.IncrementUntilEndOfTurnPlayerValue(ps.AdditionalOrder, 1)
	end
	return 0
end