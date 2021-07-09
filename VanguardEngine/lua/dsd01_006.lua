-- Vairina

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 2
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 3 then
		return a.Then, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.InOverDress and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
			return true
		end
	elseif n == 3 then
		if obj.CanSB(3) then
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
	elseif n == 3 then
		if obj.Exists(4) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 3 then
		obj.SoulBlast(3)
	end
end

function Activate(n)
	if n == 2 then
		obj.AddBattleOnlyPower(2, 10000)
		return 3
	elseif n == 3 then
		obj.ChooseRetire(4)
	end
	return 0
end
