-- Legio Wild Master, Darius

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyVC, q.Grade, 3, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(1) then
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
		obj.SoulCharge(1)
		if obj.Exists(3) then
			obj.AddBattleOnlyPower(2, 10000)
		else
			obj.AddBattleOnlyPower(2, 5000)
		end
	end
	return 0
end
