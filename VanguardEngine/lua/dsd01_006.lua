-- Vairina

function NumberOfAbilities()
	return 4
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.This
	elseif n == 3 then
		return q.Count, 2
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1
	elseif n == 2 then
		return a.OnAttack, l.PlayerRC, true, true
	elseif n == 3 then
		return a.OnBattleEnds, l.PlayerRC, true, true
	elseif n == 4 then
		return a.Then, l.PlayerRC, false, false
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.InOverDress and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
			return true
		end
	elseif n == 3 then
		if obj.InOverDress and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
			return true
		end
	elseif n == 4 then
		if obj.CanSB(3) and obj.CanRetire(4) then
			return true
		end
	end
	return false
end

function Activate(n)
	local t = 0
	if n == 2 then
		obj.AddPower(2, 10000)
		t = 4
	elseif n == 3 then
		obj.AddPower(2, -10000)
	elseif n == 4 then
		obj.SoulBlast(3)
		obj.Retire(4)
	end
	return t
end
