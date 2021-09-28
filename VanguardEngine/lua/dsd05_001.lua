-- Aurora Battle Princess, Seraph Snow

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerPrisoners, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 2
	elseif n == 5 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.HasPrison() and obj.Exists(5) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.Exists(1) then
			obj.SetAbilityPower(3, 10000)
		end
		if obj.IsPlayerTurn() and obj.Exists(2) then
			obj.SetAbilityDrive(3, 1)
		end
	elseif n == 2 then
		obj.ChooseImprison(4)
	end
	return 0
end