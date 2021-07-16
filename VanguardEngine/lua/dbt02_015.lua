-- Legio Wild Master, Darius

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyVC, q.Grade, 3, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 3
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(1) and obj.CanCB(2) then
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
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		if obj.Exists(4) then
			obj.AddBattleOnlyPower(3, 10000)
		else
			obj.AddBattleOnlyPower(3, 5000)
		end
	end
	return 0
end
