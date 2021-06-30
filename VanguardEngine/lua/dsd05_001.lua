-- Aurora Battle Princess, Seraph Snow

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 6
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerPrisoners, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 5 then
		return q.Location, l.EnemyRC, q.Count, 2
	elseif n == 6 then
		return q.Location, l.EnemyRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.CanCB(4) and obj.HasPrison() and obj.Exists(6) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(4)
	end
end

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.Exists(1) then
			obj.SetAbilityPower(3, 10000)
		else
			obj.SetAbilityPower(3, 0)
		end
		if obj.IsPlayerTurn() and obj.Exists(2) then
			obj.SetAbilityDrive(3, 1)
		else
			obj.SetAbilityDrive(3, 0)
		end
	elseif n == 2 then
		obj.ChooseImprison(5)
	end
	return 0
end