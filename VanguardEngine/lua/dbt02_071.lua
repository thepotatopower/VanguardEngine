-- Diabolos Attacker, Arwing

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if (obj.IsRearguard() or obj.IsVanguard()) and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() and obj.CanSB(2) then
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
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		if obj.InFinalRush() then
			obj.AddBattleOnlyPower(1, 15000)
		else
			obj.AddBattleOnlyPower(1, 5000)
		end
	end
	return 0
end