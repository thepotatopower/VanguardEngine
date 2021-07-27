-- Promised Brave Shooter

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 2
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(3) > 0 then
			return true
		end
	end
	return false
end

function Cost(n)
	obj.SoulBlast(1)
end

function Activate(n)
	if n == 1 then
		obj.AddBattleOnlyPower(2, obj.GetNumberOf(3) * 5000)
	end
	return 0
end