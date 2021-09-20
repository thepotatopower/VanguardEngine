-- Vairina

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.InOverDress() and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
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
	if n == 2 then
		obj.AddBattleOnlyPower(2, 10000)
		if obj.CanSB(2) and obj.YesNo("Soul Blast 2 to retire one enemy rear-guard?") then
			obj.SoulBlast(2)
			obj.ChooseRetire(3)
		end
	end
	return 0
end
