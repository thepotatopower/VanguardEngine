-- Cataclysmic Bullet of Dust Storm, Randor

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Count, 2, q.Other, o.OrLess
	elseif n == 3 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Heavy Artillery of Dust Storm, Eugene") and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() and obj.Exists(2) and obj.CanCB(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddToSoul(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.SoulCharge(1)
		obj.AddBattleOnlyPower(4, 5000)
	end
	return 0
end