-- Promised Brave Shooter

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(2) > 0 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddBattleOnlyPower(1, obj.GetNumberOf(2) * 5000)
	end
	return 0
end