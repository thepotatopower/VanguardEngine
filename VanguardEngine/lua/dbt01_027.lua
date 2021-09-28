-- Cataclysmic Bullet of Dust Storm, Randor

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Count, 1, q.Min, 0
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt
	elseif n == 2 then
		return a.OnAttack, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Heavy Artillery of Dust Storm, Eugene") and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() and not obj.Exists(2) then
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

function Activate(n)
	if n == 1 then
		obj.ChooseAddToSoul(1)
	elseif n == 2 then
		obj.SoulCharge(1)
		obj.AddBattleOnlyPower(3, 5000)
	end
	return 0
end