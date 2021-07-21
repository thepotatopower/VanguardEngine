-- Form Up, O Chosen Knights

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 7
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 2
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 3
	elseif n == 5 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 4
	elseif n == 6 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	elseif n == 7 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(1) and obj.CanSB(2) then
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
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		if obj.Exists(3) then
			obj.ChooseAddTempPower(6, 5000)
		end
		if obj.Exists(4) then
			obj.Draw(1)
		end
		if obj.Exists(5) then
			obj.ChooseAddDrive(7, 1)
		end
	end
	return 0
end