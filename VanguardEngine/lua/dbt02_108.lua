-- Shieldfisher Dragon

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.UnitsCalledThisTurn
	elseif n == 2 then
		return q.Location, l.UnitsCalledFromHandThisTurn
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() or obj.IsVanguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		if obj.GetNumberOf(1) > obj.GetNumberOf(2) then
			obj.AllowAttack(3)
		else
			obj.DisableAttack(3)
		end
	end
	return 0
end