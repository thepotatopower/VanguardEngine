-- Blaze Fist Monk, Enten

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1, q.Other, o.NotThis
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() and obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
		obj.Retire(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddTempPower(3, 10000)
	end
	return 0
end