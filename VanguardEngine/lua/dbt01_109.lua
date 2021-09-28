-- Hydrolic Ram Dragon

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
		return q.Location, l.Bind, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsAttackingUnit() and obj.IsRearguard() and obj.Exists(2) and obj.TargetIsEnemyVanguard() then
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
		obj.AddBattleOnlyPower(1, 5000)
	end
	return 0
end