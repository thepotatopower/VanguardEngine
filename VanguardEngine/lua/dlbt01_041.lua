-- Advent Stroke, Schedael

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Bind, q.Count, 1, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.CB, 1, p.AddToSoul, 1
	elseif n == 2 then
		return a.Cont, p.IsMandatory, p.BlackWings
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.IsRearguard() then
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
		obj.Draw(1)
		obj.Select(2, "to give grade +1.")
		obj.AddUntilEndOfTurnValue(3, cs.BonusGrade, 1)
	elseif n == 2 then
		obj.Resist(1)
		obj.AddContinuousState(1, cs.CannotBeAttackedByRearguard)
	end
	return 0
end