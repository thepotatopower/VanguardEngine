-- Blaze Maiden, Tanya

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 0, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.CanCB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.CanSB(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		if obj.Exists(4) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	elseif n == 2 then
		obj.SoulBlast(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(2)
	elseif n == 2 then
		obj.ChooseAddTempPower(4, 5000)
	end
	return 0
end