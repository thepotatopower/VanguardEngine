-- Blaster Dark

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnVC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.Retire, 1
	elseif n == 2 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.Retire, 1
	elseif n == 3 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnVC() and obj.CanCB(1) and obj.CanRetire(2) then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() and obj.CanCB(1) and obj.CanRetire(2) then
			return true
		end
	elseif n == 3 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 or n == 2 then
		if obj.CanRetire(3) then
			return true
		end
	elseif n == 3 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 or n == 2 then
		obj.CounterBlast(1)
		obj.ChooseRetire(2)
	end
end

function Activate(n)
	if n == 1 or n == 2 then
		obj.ChooseRetire(3) 
		obj.AddDrive(4, 1)
	elseif n == 3 then
		if obj.IsPlayerTurn() and obj.PlayerRetiredThisTurn() then
			obj.SetAbilityPower(4, 5000)
		else
			obj.SetAbilityPower(4, 0)
		end
	end
	return 0
end
