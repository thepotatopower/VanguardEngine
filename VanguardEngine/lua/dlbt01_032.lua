-- Brainy Prayer, Bibuel

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Bind, q.UnitType, u.Trigger, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.GC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.SB, 1, p.Retire, 1
	elseif n == 2 then
		return a.Cont, p.IsMandatory, p.WhiteWings
	end
end

function CheckCondition(n)
	if n == 1 then
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
		obj.AddUntilEndOfTurnValue(4, cs.BonusGrade, 1)
	elseif n == 2 then
		obj.SetAbilityPower(3, 5000)
		obj.SetAbilityShield(3, 5000)
	end
	return 0
end